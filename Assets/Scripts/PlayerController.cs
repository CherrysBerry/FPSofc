using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed = 5f;
    public Transform footPosition;
    public float mouseSensivity;
    public float jumpForce = 5f;
    [SerializeField] public float runSpeed = 10f;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Camera mainCamera;
    private Vector2 lookInput;
    private float cameraPitch = 0f;
    private bool isGrounded = false;
    private bool isRunning = false;

    //Meus dados de Jogador

    public int kills;
    public int hp = 100;
    public GameObject telaDano;
    private TMP_Text textoHp;
    private TMP_Text textoKills;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        textoHp = GameObject.FindGameObjectWithTag("HpTexto").GetComponent<TMP_Text>();
        textoKills = GameObject.FindGameObjectWithTag("KillText").GetComponent<TMP_Text>();
    }


    // Update is called once per frame
    void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        isRunning = playerInput.actions["Sprint"].ReadValue<float>() > 0;
        RotateCamera();
        RotatePlayer();
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            Jump();
        }
    }

    void RotateCamera()
    {
        cameraPitch -= lookInput.y * mouseSensivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        mainCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }
    void RotatePlayer()
    {
        float yam = lookInput.x * mouseSensivity;
        transform.Rotate(Vector3.up * yam);
    }

    private void Move()
    {
        Vector3 cameraFoward = mainCamera.transform.forward;
        cameraFoward.y = 0;
        cameraFoward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementDirection = (cameraFoward * movementInput.y + cameraRight * movementInput.x).normalized;

        float currentSpeed;

        //Versão IF Ternário - float currentSpeed = isRunning ? runSpeed : speed;
        if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = speed;

        Vector3 displacement = movementDirection * currentSpeed * Time.deltaTime;


        rb.MovePosition(transform.position + displacement);

    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(footPosition.position, Vector3.down, 0.05f);
        Move();
    }
    private void OnDrawGizmos()
    {
        if (footPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(footPosition.position, footPosition.position + Vector3.down * 0.05f);
        }
    }

    void Dano()
    {
        hp = hp - 10;
        AtualizaDados();
        telaDano.SetActive(true);
        if (hp <= 0)
        {
            Morrer();
        }
    }
    public void Morrer()
    {
        SceneManager.LoadScene(2);
    }

    void GanharVida()
    {
        hp = hp + 20;
        if (hp > 99)
        {
            hp = 100;
        }
    }

    public void AtualizaDados()
    {
        textoHp.text = hp.ToString()+"/100";
        textoKills.text = kills.ToString();
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "AtaqueInimigo")
        {
            Dano();
        }
        if (colidiu.gameObject.tag == "CaixaVida")
        {
            GanharVida();
            Destroy(colidiu.gameObject);
        }
        if (colidiu.gameObject.tag == "CaixaVitória")
        {
            SceneManager.LoadScene("Victory");
        }
    }

}
