using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button apoDiez;
    public Button apoCien;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text txtBnco;
    public Text txtApuesta;
    public Text txtProb;

    int banca = 1000;
    int apuesta = 0;

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
        int cartap = 0;//variable para recoger el valor  el valor de las cartas en el palo
        for (int i = 0; i < 52; i++)//rellenamos los valores de las cartas
        {
            cartap++;

            if (cartap <= 10)//la carta tiene valores menores o iguales a 10
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
                if (cartap == 13)//para ubicar el siguiente palo
                {
                    cartap = 0;//para reiniciarlo
                }
                values[i] = 10;// para la que son 10
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
            quitSprite = faces[i];//desordenamos con la misma variable aleatoria para que la carta coincida con su valor
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
                apoDiez.interactable = false;
                apoCien.interactable = false;
                banca = banca + 0;
                apuesta = 0;
                transaccion();

            }
            //luego con el player
            if (player.GetComponent<CardHand>().points == 21 || dealer.GetComponent<CardHand>().points > 21)
            {
                stickButton.interactable = false;
                hitButton.interactable = false;
                apoDiez.interactable = false;
                apoCien.interactable = false;
                finalMessage.text = "Has ganado";
                banca = banca + apuesta * 2;
                apuesta = 0;
                transaccion();
            }
            //si hay empate
            if(player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21)
            {
                stickButton.interactable = false;
                hitButton.interactable = false;
                apoDiez.interactable = false;
                apoCien.interactable = false;
                finalMessage.text = "Empate";
                banca = banca + apuesta;
                apuesta = 0;
                transaccion();
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
        //Probabilidad de que el jugador obtenga más de 21 si pide una carta

        int cPos;//variable de los casos posibles
        float prob;//probabilidades entre el minimo 0 y el maximo 1

        cPos = 13 - (21 - player.GetComponent<CardHand>().points);//del as al rey hay 13 - (21(el numero que se pide) - los puntos que lleva el jugador)
        prob = cPos / 13f;// casos posibles / casos totales 

        //redondeamos dado el caso de ajuste para evitar errores
        if (prob < 0)
        {
            prob = 0;
        }
        if (prob > 1)
        {
            prob = 1;
        }
        if (player.GetComponent<CardHand>().points <= 11)//si los puntos son <= no hay carta que te permita llegar a 21, por lo tanto la prob es 0
        {
            prob = 0;
        }
        txtProb.text = "La probabilidad de obtener más de 21 es: " + (prob * 100).ToString() + " %";//imprimimos el resultado obtenido

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
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
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true); // se gira la carta del dealer igual

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
            apoDiez.interactable = false;
            apoCien.interactable = false;

            banca = banca + 0;
            apuesta = 0;
            transaccion();
        }
        else if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Has ganado";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apoDiez.interactable = false;
            apoCien.interactable = false;

            banca = banca + apuesta * 2;
            apuesta = 0;
            transaccion();
        }


    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();

        }

        if (dealer.GetComponent<CardHand>().points >= 17)
        {

            //primero se comprueba si hay empate
            if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
            {
                finalMessage.text = "Empate";
                banca = banca + apuesta;
                apuesta = 0;
                transaccion();
            }
            //luego si ha ganado el jugador
            if (dealer.GetComponent<CardHand>().points > 21 || dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
            {
                finalMessage.text = "Has ganado";
                banca = banca + apuesta * 2;
                apuesta = 0;
                transaccion();

            }
            else//si no, ha ganado el dealer
            {
                finalMessage.text = "Has perdido";
                banca = banca + 0;
                apuesta = 0;
                transaccion();
            }
            //bloqueamos los botones
            hitButton.interactable = false;
            stickButton.interactable = false;
            apoDiez.interactable = false;
            apoCien.interactable = false;


        }





    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        apoDiez.interactable = true;
        apoCien.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    public void aposDiez()
    {
        if (banca >= 10)
        {
            apuesta = apuesta + 10;
            banca = banca - 10;
            transaccion();
        }
    }
    public void aposCien()
    {
        if (banca >= 100)
        {
            apuesta = apuesta + 100;
            banca = banca - 100;
            transaccion();
        }
    }
    private void transaccion()
    {
        txtBnco.text = banca.ToString();
        txtApuesta.text = apuesta.ToString();

    }

}
