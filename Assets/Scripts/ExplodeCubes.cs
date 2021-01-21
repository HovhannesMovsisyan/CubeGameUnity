
using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject restartButton, explosion;
    private bool _collisionSet;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube") && !_collisionSet)
        {
            for(int i=collision.transform.childCount-1; i>=0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(83f, Vector3.up, 5f);
                child.SetParent(null);
            }
            restartButton.SetActive(true);
            Camera.main.transform.position -= new Vector3(1f, 1f, 3f);
            Camera.main.gameObject.AddComponent<CameraShake>();

            GameObject newVfx = Instantiate(explosion, new Vector3(collision.contacts[0].point.x, 
                collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity) as GameObject;
            Destroy(newVfx, 2.5f);

            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }
}
