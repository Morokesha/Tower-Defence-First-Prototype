using UnityEngine;

public static class UtillClass
{
    private static Camera mainCamera;

    public static Vector3 GetMouseWorldPosition() {
        if (mainCamera == null) mainCamera = Camera.main;
        // Ѕерем координаты курсора относительно нашей камеры 
        // а не экрана как возвращает нам свойство Input.mousePosition
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }
    //ћетод дл€ задани€ углов вращени€ снар€дов
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }
}
