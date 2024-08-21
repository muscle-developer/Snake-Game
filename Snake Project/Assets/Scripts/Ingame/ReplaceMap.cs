using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceMap : MonoBehaviour
{
    private void OnTriggerExit(Collider collision)
    {
        if(!collision.CompareTag("Area"))
            return;

        switch(transform.tag)
        {
            case "Floor":
                Vector3 playerPos = GameManager.Instace.player.transform.position;
                Vector3 mapPos = this.transform.position;

                float diffX = Mathf.Abs(playerPos.x - mapPos.x);
                float diffZ = Mathf.Abs(playerPos.z - mapPos.z);

                float dirX = diffX < 0 ? -1 : 1;
                float dirZ = diffX < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffZ = Mathf.Abs(diffX);

                if(diffX > diffZ)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if(diffX < diffZ)
                {
                    transform.Translate(Vector3.up * diffZ * 40);
                }
            break;
            case "Enemy":
            break;
        }
    }
}   
