using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;   //defines the value of the satrting health       
    public Slider m_Slider; //used to reference the slider created in the scene                        
    public Image m_FillImage;   //used to acess the image present inside the slider                    
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    
    
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  
    private bool m_Dead;            


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>(); //gets component from a original object "m_ExplosionPrefab" and clones the object
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>(); //gets the explosion audio from the audiosource 

        m_ExplosionParticles.gameObject.SetActive(false); //turns down the explosion gameobject as it doesnt have to explode at the start
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }
    

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount; //reduces the health according to the ammount of damage
        SetHealthUI();
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
    }
}