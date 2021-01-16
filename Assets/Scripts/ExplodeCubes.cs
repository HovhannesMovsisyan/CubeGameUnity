
using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject restartButton;
    private bool _collisionSet;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube") && !_collisionSet)
        {
            for(int i=collision.transform.childCount-1; i>=0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(150f, Vector3.up, 5f);
                child.SetParent(null);
            }
            restartButton.SetActive(true);
            Camera.main.transform.position -= new Vector3(1f, 1f, 3f);
            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }
}
