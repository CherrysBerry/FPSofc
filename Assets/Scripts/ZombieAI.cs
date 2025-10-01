using UnityEngine;
using System.Collections.Generic;

public class ZombieAI : MonoBehaviour
{
    private Animator anim;
    private Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public ZombieState currentState;
    public GameObject personagem;
    public GameObject meuAtaque;
    public int hp = 25;
    public bool zumbiVivo = true;
    public List<AudioClip> sonsZumbi;


    void Start()
    {
        personagem = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zumbiVivo == true)
        {
            Presenca();
            if (currentState == ZombieState.Walking)
            {
                Seguir();
            }
            if(currentState == ZombieState.Attacking)
            {
                anim.SetTrigger("atk");
                OlharPersonagem();
            }
        }
    }

    void Presenca()
    {
        float distanciaPersonagem = Vector3.Distance(personagem.transform.position, transform.position);
        if (distanciaPersonagem < 2)
        {
            currentState = ZombieState.Attacking; 
        }
        else if (distanciaPersonagem < 6)
        {
            currentState = ZombieState.Walking;
            anim.SetBool("walk", true); 
        }
        else if (distanciaPersonagem > 15)
        {
            currentState = ZombieState.Idle;
            anim.SetBool("walk", false);
        }
    }
    void Seguir()
    {
        Vector3 vetorCorrigido = new Vector3(personagem.transform.position.x, transform.position.y, personagem.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, vetorCorrigido, 0.02f);
        transform.LookAt(vetorCorrigido);
    }

    void OlharPersonagem()
    {
        Vector3 vetorCorrigido = new Vector3(personagem.transform.position.x, transform.position.y, personagem.transform.position.z);
        transform.LookAt(vetorCorrigido);
    }

    public void IniciarAtaque()
    {
        //Ativando o meu ataque
        meuAtaque.SetActive(true);
    }
    public void EncerarAtaque()
    {
        //Encerrando o meu ataque
        meuAtaque.SetActive(false);
    }
    public void TomeiDano(int tipoDano)
    {
        meuAtaque.SetActive(false);
        hp = hp - tipoDano;
        if (hp < 0)
        {
            anim.SetBool("die", true);
        }
        anim.SetTrigger("dmg");
    }

    private void OnTriggerEnter(Collider colidir)
    {
        if (colidir.gameObject.tag == "Bullet")
        {
            if(zumbiVivo == true)
            {
                Destroy(colidir.gameObject);
                TomeiDano(10);
            }
        }
    }
    public void Morreu()
    {
        zumbiVivo = false;
        meuAtaque.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;
        PlayerController.instance.kills++;
        PlayerController.instance.AtualizaDados();
        //Destroy(this.gameObject);
    }
    public void SomZumbi(int tipoSom)
    {
        GetComponent<AudioSource>().clip = sonsZumbi[tipoSom];
        GetComponent<AudioSource>().Play();
    }
    
}
