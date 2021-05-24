using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
   
    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int cartap = 0;//variable para el valor de las cartas en el palo
        for(int i =0; i< 52; i++)
        {
            cartap++;

            if(cartap <= 10)//la carta tiene valores mayores o iguales a 10, solo quedan 3 cartas
            {
                if (cartap != 1)//si no es un as
                {
                    values[i] = cartap;//el valor es el perteneciente a la carta
                }
                else
                {
                    //si es un as
                    values[i] = 11;
                }

               
            }
            else//si se cumple la condicion establecida
            {
                if (cartap == 13)
                {
                    cartap = 0;
                }
                values[i] = 10;
            }
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        int quit; //se quita el numero para poner otra aleatoria(aux)
        Sprite quitSprite;//se quita el numero para el sprite
        int random; //variable para guardar el numero random



        //procedemos con el proceso almacenandolo en arrays
        for (int i = 0; i <= values.Length - 1; i++)
        {
            random = Random.Range(0, 52); //se genera el numero aleatorio
            quit = values[i];//se guarda en la auxuiliar
            values[i] = values[random];//ponemos el numero actual en el random
            values[random] = quit;//ponemos el valor random de la aux

            //Para los Sprites
            quitSprite = faces[i];
            faces[i] = faces[random];
            faces[random] = quitSprite;
        }
    }

    void StartGame()
    {

        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            //empezamos con el dealer
            if (dealer.GetComponent<CardHand>().points == 21 || player.GetComponent<CardHand>().points > 21)//si el dealer llega a 21 o el jugador se pasa de 21
            {
                //quitamos la interaccion posible con el seguimiento del juego
                hitButton.interactable = false;
                stickButton.interactable = false;
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true); // se gira la acrta del dealer
                finalMessage.text = "Has perdido";
                
            }
            if (player.GetComponent<CardHand>().points == 21 || dealer.GetComponent<CardHand>().points > 21)
            {
                stickButton.interactable = false;
                hitButton.interactable = false;
                finalMessage.text = "Has ganado";
               
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true); // se gira la carta del dealer en la casilla 0
        //Repartimos carta al jugador
        PushPlayer();
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points > 21)//si se ha pasado de 21
        {
            finalMessage.text = "Has perdido";
            stickButton.interactable = false;
            hitButton.interactable = false;
            
        }
        else if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Has ganado";
            stickButton.interactable = false;
            hitButton.interactable = false;
            
        }
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
}
