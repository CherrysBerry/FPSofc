using UnityEngine;

public class Arma : MonoBehaviour
{
    public GameObject projetil;
    public GameObject pontoSaida;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Disparo();
    }
    void Disparo()
    {
        GameObject Tiro = Instantiate(projetil, pontoSaida.transform.position, Quaternion.identity);
        Tiro.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
        Destroy(Tiro, 1f);
        
    }
}
