using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	private Transform playerTransform;

	void LateUpdate()
    {
		FollowCamera();
    }

    private void FollowCamera()
    {
        if (playerTransform != null)
        {
			transform.position = playerTransform.position;
		}
    }

    public void SetTarget(Transform target)
	{
		Debug.Log(target);
		playerTransform = target;
	}
}