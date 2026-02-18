using UnityEngine;

public class ShootCube : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    GameObject bullet;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                bullet = Instantiate(bulletPrefab, Camera.main.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce((hit.point-Camera.main.transform.position).normalized * 50f,ForceMode.Impulse);
            }
        }
    }

}
