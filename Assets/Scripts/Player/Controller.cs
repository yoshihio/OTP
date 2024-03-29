﻿using Assets.GameSystem.Constant;
using UnityEngine;
using UnityEngine.UI;
using GameSystem.Service;
using GameSystem.Entity;
using TMPro;

namespace Player
{
    public class Controller : GameConstants
    {
        private bool isGrounded = false;
    
        Rigidbody m_rb;
    
        public GameObject main_camera;
        // Character Joystick Movement Variable
        private Transform m_Cam;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private float movement_speed = 7f;
        Quaternion targetRotation;

        public AudioClip audioClipJump;
        public AudioClip audioClipLand;

        private AudioSource cameraAudioSource;
        private AudioSource playerAudioSource;

        public bool m_Jump;
        private float verticalVelocity;
        public float HInput;
        public float VInput;

        void Awake() {
            GameSettings preference = PreferencesService.LoadSettings();
            cameraAudioSource = main_camera.GetComponent<AudioSource>();
            playerAudioSource = this.gameObject.GetComponent<AudioSource>();
            Debug.Log(((float) preference.Volume/100f));
            playerAudioSource.volume = ((float) preference.Volume/100f);
            cameraAudioSource.volume = ((float) preference.Volume/100f);
        }

        void Start(){
            cameraAudioSource.Play();
        }
        void Update()
        {
            if(isGrounded && m_Jump && m_rb.velocity.y < 0.001f){
                isGrounded = false;
                playerAudioSource.clip = audioClipJump;
                playerAudioSource.Play();
                this.GetComponent<Rigidbody>().AddForce(Vector3.up * 400f);
            }
            else if (!isGrounded && m_Jump && m_rb.velocity.y < 0)
            {
                SetGravityScale(1f);
                verticalVelocity = GetGravityScale() * Time.deltaTime * 100f;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0, -verticalVelocity, 0);
            }
        
            if(!m_Jump)
            {
                SetGravityScale(1f);
            }

            Data.Instance.starText.GetComponent<TextMeshProUGUI>().text = Data.Instance.starCounter.ToString();
        }
        void OnEnable ()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.useGravity = false;
        }
 
        void FixedUpdate ()
        {
            Vector3 gravity = GetGlobalGravityScale() * GetGravityScale() * Vector3.up;
            m_rb.AddForce(gravity, ForceMode.Acceleration);

            // Character Movement
            if(null != m_Cam){
                // Calculate camera realative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = VInput * m_CamForward + HInput * m_Cam.right;
            }
            else{
                //we use world-relative directions in the case of no main camera
                m_Move = VInput * Vector3.forward + HInput * Vector3.right;
            }

            Vector3 targetDirection = new Vector3(HInput, 0f, VInput);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            if(0 != HInput  || 0f != VInput){
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            }

            this.transform.rotation = targetRotation;
            this.transform.position += Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up) * VInput * Time.deltaTime * movement_speed;
            this.transform.position += Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) * HInput * Time.deltaTime * movement_speed;

        }

        private void OnCollisionEnter(Collision other) {
            if("Ground".Equals(other.gameObject.tag)){
                isGrounded = true;
                playerAudioSource.clip = audioClipLand;
                playerAudioSource.Play();
            }
            else if ("Mushroom".Equals(other.gameObject.tag))
            {
                isGrounded = false;
                this.GetComponent<Rigidbody>().AddForce(Vector3.up * 12f, ForceMode.Impulse);
            }
            else if ("Mushroom-2".Equals(other.gameObject.tag))
            {
                isGrounded = false;
                this.GetComponent<Rigidbody>().AddForce(Vector3.up * 18f, ForceMode.Impulse);
            }
            else if ("Star".Equals(other.gameObject.tag))
            {
                Data.Instance.starCounter++;
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other) {
            //Kalo Pake Double Collider yang 1nya trigger
            // if ("Mushroom".Equals(other.gameObject.tag))
            // {
            //     isGrounded = false;
            //     this.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
            // }
            if("Water".Equals(other.gameObject.tag)){
                if(null == EventService.LoadCheckPoint().XPos){
                    this.transform.position = new Vector3(46.51f, 1.83f, -0.07f);
                }
                else{
                    this.transform.position = new Vector3(EventService.LoadCheckPoint().XPos, EventService.LoadCheckPoint().YPos, EventService.LoadCheckPoint().ZPos);
                }
            }
        }
    }
}