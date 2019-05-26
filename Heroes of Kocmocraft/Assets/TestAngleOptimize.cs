using UnityEngine;

public class TestAngleOptimize : MonoBehaviour
{
    public Transform target;
    private Transform myCamera;
    public Vector3 difference1, difference2;
    public float distSqr1, distSqr2;
    public float direction1, direction2;
    public float angle1, angle2;
    void Start()
    {
        myCamera = transform;
    }
    void Update()
    {
        for (int i = 0; i < 10000; i++)
        {
            //Calculate1();
            Calculate2();
        }
    }
    void Calculate1()
    {
        difference1 = target.position - myCamera.position; // 137%
        distSqr1 = Vector3.SqrMagnitude(difference1);
        direction1 = Vector3.Dot(difference1.normalized, myCamera.forward);
        angle1 = Vector3.Angle(difference1.normalized, myCamera.forward); // 127%
    }
    void Calculate2()
    {
        difference2 = myCamera.InverseTransformPoint(target.position); // 100%
        distSqr2 = Vector3.SqrMagnitude(difference2);
        direction2 = Vector3.Dot(difference2.normalized, Vector3.forward);
        //angle2 = Vector3.Angle(difference2.normalized, Vector3.forward); // 100%
    }
}
