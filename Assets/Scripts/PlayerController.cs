using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    

    private Rigidbody2D _rb; //para que nuestro player avance
    private Animator _animator; //maneja la animación
    public float JumpForce = 10; //para saltar
    public float Velocity = 10;
    public GameObject BullerPrefab;//para la bala

    private const string ANIMATOR_STATE = "Estado"; //para el estado de la animación
    private static readonly int ANIMATION_IDLE = 0;
    private static readonly int ANIMATION_RUN = 1;
    private static readonly int ANIMATION_JUMP = 2; // definimos los estados
    private static readonly int ANIMATION_SLICE = 3; //deslizarse
    //private static readonly int ANIMATION_DEAD = 4; //morir
    private static readonly int ANIMATION_SHOOT = 5; //disparar
    //private static readonly int ANIMATION_SHOOTRUN = 56; //correrdisparar

    private static readonly int RIGHT = 1; //irá a la derecha
    private static readonly int LEFT = -1; //irá a la izquierda
    private SpriteRenderer _spriteRenderer;//llamamos al Sprite Renderer
    void Start()
    {
        
        _rb = GetComponent<Rigidbody2D>();//llamamos a rigidbody
        _animator = GetComponent<Animator>();//llamamos al animator
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y); //le decimos que se quede quieto
        ChangeAnimation(ANIMATION_IDLE);

        if (Input.GetKey(KeyCode.RightArrow)) //si presionamos flecha derecha
        {
            Desplazarse(RIGHT);
            

        }
        if (Input.GetKey(KeyCode.LeftArrow)) //si presionamos flecha derecha
        {
            Desplazarse(LEFT);
            
        }
        if (Input.GetKey(KeyCode.C)) //si presionamos c
        {
            Deslizarse();

        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            Disparar();
            ChangeAnimation(ANIMATION_SHOOT);
        }
        if (Input.GetKeyUp(KeyCode.Space)) //si presionamos flecha espacio
        {

            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

            ChangeAnimation(ANIMATION_JUMP);
        }
    }
    private void Disparar() //código para crear gameObjets en tiempo de ejecución
    {
        var x = this.transform.position.x;
        var y = this.transform.position.y;

        var bulletGO = Instantiate(BullerPrefab, new Vector2(x, y), Quaternion.identity) as GameObject;
        
        //controller.SetPlayerController(this);
        if (_spriteRenderer.flipX)//hacer que las balas vayan a la izquierda al invertir dirección
        {
            var controller = bulletGO.GetComponent<BulletController>();
            controller.Velocidad = controller.Velocidad * -1;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.gameObject.tag;
        Debug.Log("On Trigger: " + tag);
    }
    private void Deslizarse()
    {
        ChangeAnimation(ANIMATION_SLICE);
    }
    private void Desplazarse(int position)
    {
        _rb.velocity = new Vector2(Velocity * position, _rb.velocity.y);//le decimos que se dezplace
        _spriteRenderer.flipX = position == LEFT;
        ChangeAnimation(ANIMATION_RUN);
    }

    public void ChangeAnimation(int animation)
    {
        _animator.SetInteger(ANIMATOR_STATE, animation);

    }
    
    
    
}
