using UnityEngine;

public class MathCalcul : MonoBehaviour {

    private GameObject cubeTest;
    private GameObject cube2Test;

    private Transform p1Test;
    private Transform p2Test;
    // Use this for initialization
    void Start() {
        cubeTest = GameObject.Find("Cube_test");
        p1Test = cubeTest.transform;
        cube2Test = GameObject.Find("Cube_2_test");
        p2Test = cube2Test.transform;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Angle : " + GetAngle(p1Test.localPosition, p2Test.localPosition));
            Debug.Log("Distance : " + GetDistance(p1Test.localPosition, p2Test.localPosition));
        }
    }

    // Distance entre 2 points
    public static double GetDistance(Vector3 p1, Vector3 p2) {
        double d1 = Mathf.Sqrt((Mathf.Pow((p1.x - p2.x), 2)) + (Mathf.Pow((p1.y - p2.y), 2)));
        float alpha = Mathf.Atan((p1.z - p2.z) / (p1.x - p2.x));
        double d2 = d1 / Mathf.Cos(alpha);
        return d2;
    }

    public static Vector3 GetAngle(Vector3 p1, Vector3 p2) {
        float angleX = CalculAngleX(p1, p2) * Mathf.Rad2Deg;
        float angleY = CalculAngleY(p1, p2) * Mathf.Rad2Deg;
        float angleZ = CalculAngleZ(p1, p2) * Mathf.Rad2Deg;

        return new Vector3(angleX, angleY, angleZ);
    }
    private static float CalculAngleX(Vector3 p1, Vector3 p2) {
        // Calcul du point p3 pour les calcul d'angle
        float x, y, z;
        x = p2.x;
        // même hauteur
        y = p1.y;
        z = p1.z;

        Vector3 p3 = new Vector3(x, y, z);
        Debug.Log("Calcul angle X :" + p3);
        return Mathf.Atan(Mathf.Abs(p2.z - p3.z) / Mathf.Abs(p1.x - p3.x));

    }

    private static float CalculAngleY(Vector3 p1, Vector3 p2) {
        float x, y, z;
        y = p2.y;
        z = p1.z;
        x = p1.x;
        Vector3 p3 = new Vector3(x, y, z);
        Debug.Log("Calcul angle Y :" + p3);
        return Mathf.Atan(Mathf.Abs(p2.z - p3.z) / Mathf.Abs(p1.y - p3.y));
    }

    private static float CalculAngleZ(Vector3 p1, Vector3 p2) {
        float x, y, z;
        x = p1.x;
        y = p1.y;
        z = p2.z;
        Vector3 p3 = new Vector3(x, y, z);
        Debug.Log("Calcul angle Z :" + p3);
        return Mathf.Atan(Mathf.Abs(p2.y - p3.y) / Mathf.Abs(p1.z - p3.z));
    }

}
