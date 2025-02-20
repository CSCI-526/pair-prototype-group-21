using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // ��Ҷ���
    public float smoothSpeed = 5f; // ����������ƽ���ٶ�
    public Vector3 offset = new Vector3(0, 0, 0); // ƫ������Z��Ϊ -10 ������������

    void LateUpdate()
    {
        if (player != null)
        {
            // Ŀ��λ�� = ���λ�� + ƫ����
            Vector3 targetPosition = player.position + offset;

            // ƽ���ƶ������
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
