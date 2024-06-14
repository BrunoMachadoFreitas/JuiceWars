using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referência ao objeto que a câmera seguirá
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera

    // Referência para a área de jogo
    public GameObject playingArea;
    public GameObject BackgroundplayingArea;

    // Limites para a posição da câmera
    private float minX, maxX, minY, maxY;

    void Start()
    {
        // Calcula os limites da área de jogo
        target = Player_Main.instance.transform;
        playingArea = RoundsManager.instance.PlayingAreaAux;
        //BackgroundplayingArea = Instantiate(playingArea, new Vector3(-489.132446f, 0.738508701f, 0), Quaternion.identity);
        CalculateBounds();

        

    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Calcula a posição desejada da câmera
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // Limita a posição desejada dentro dos limites da área de jogo
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

            // Interpola suavemente entre a posição atual da câmera e a posição desejada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Atualiza a posição da câmera
            transform.position = smoothedPosition;
        }
    }

    // Método para calcular os limites da área de jogo
    private void CalculateBounds()
    {
        if (playingArea != null)
        {
            // Obtém o componente de renderização da área de jogo
            Renderer renderer = playingArea.GetComponent<Renderer>();

            // Calcula os limites da área de jogo
            minX = renderer.bounds.min.x + Camera.main.orthographicSize * Screen.width / Screen.height;
            maxX = renderer.bounds.max.x - Camera.main.orthographicSize * Screen.width / Screen.height;
            minY = renderer.bounds.min.y + Camera.main.orthographicSize;
            maxY = renderer.bounds.max.y - Camera.main.orthographicSize;
        }
    }
}
