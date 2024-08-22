using UnityEngine;

public class ReplaceMap : MonoBehaviour
{
    private void OnTriggerExit(Collider collision)
    {
        if(!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 mapPos = this.transform.position;

        switch(transform.tag)
        {
            case "Floor":
                float diffX = playerPos.x - mapPos.x;
                float diffZ = playerPos.z - mapPos.z;

                float dirX = diffX < 0 ? -1 : 1;
                float dirZ = diffZ < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffZ = Mathf.Abs(diffZ);

                if (diffX > diffZ)
                    transform.Translate(Vector3.right * dirX * 40);
                else if (diffX < diffZ)
                    transform.Translate(Vector3.forward * dirZ * 40);

                Debug.Log("Moving map to direction: " + (diffX > diffZ ? "X" : "Z"));
            break;
            case "Enemy":
            break;
        }
    }
}   
