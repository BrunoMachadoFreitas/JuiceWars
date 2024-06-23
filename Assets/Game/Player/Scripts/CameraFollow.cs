using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Refer�ncia ao objeto que a c�mera seguir�
    public float smoothSpeed = 0.125f; // Velocidade de suaviza��o do movimento da c�mera

    // Refer�ncia para a �rea de jogo
    public GameObject playingArea;
    public GameObject BackgroundplayingArea;

    // Limites para a posi��o da c�mera
    private float minX, maxX, minY, maxY;

    void Start()
    {
        // Calcula os limites da �rea de jogo
        target = Player_Main.instance.transform;
        playingArea = RoundsManager.instance.PlayingAreaAux;
        //BackgroundplayingArea = Instantiate(playingArea, new Vector3(-489.132446f, 0.738508701f, 0), Quaternion.identity);
        CalculateBounds();

        

    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Calcula a posi��o desejada da c�mera
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // Limita a posi��o desejada dentro dos limites da �rea de jogo
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

            // Interpola suavemente entre a posi��o atual da c�mera e a posi��o desejada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Atualiza a posi��o da c�mera
            transform.position = smoothedPosition;
        }
    }

    // M�todo para calcular os limites da �rea de jogo
    private void CalculateBounds()
    {
        if (playingArea != null)
        {
            // Obt�m o componente de renderiza��o da �rea de jogo
            Renderer renderer = playingArea.GetComponent<Renderer>();

            // Calcula os limites da �rea de jogo
            minX = renderer.bounds.min.x + Camera.main.orthographicSize * Screen.width / Screen.height;
            maxX = renderer.bounds.max.x - Camera.main.orthographicSize * Screen.width / Screen.height;
            minY = renderer.bounds.min.y + Camera.main.orthographicSize;
            maxY = renderer.bounds.max.y - Camera.main.orthographicSize;
        }
    }
}
