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
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    /// <summary>
	/// Changes the value and color of the slider according to the health lost
	/// </summary>
    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    /// <summary>
    /// makes the tank innactive
    /// </summary>
    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position; //makes the explosion happen in the tank current position
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        gameObject.SetActive(false);

    }
}
