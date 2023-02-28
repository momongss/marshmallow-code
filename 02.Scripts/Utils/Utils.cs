using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utils
{
    static string debugkey = "LEETAEHYUNG ";

    public static Vector3 GetRandomPos(Vector3 center, float minDistance, float maxDistance)
    {
        Vector3 dir = new Vector3(
            Random.Range(-360f, 360f),
            0f,
            Random.Range(-360f, 360f)).normalized * Random.Range(minDistance, maxDistance);

        return center + dir;
    }

    public static int Sum(int[] array)
    {
        int sum = 0;
        for (int i = 0; i < array.Length; ++i)
        {
            sum += array[i];
        }

        return sum;
    }

    public static void PrintDebug(string msg)
    {
        Debug.Log(debugkey + msg);
    }

    public static void CreateDirectory(string path)
    {
        if (System.IO.Directory.Exists(path))
        {
            return;
        }

        System.IO.Directory.CreateDirectory(path);
    }
    public static void Look(Transform transform, Vector3 lookDir, float turningSpeed)
    {
        Vector3 newDir = Vector3.RotateTowards(transform.forward, lookDir, turningSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Camera 방향에 맞는 Horizontal, Vertical 방향을 얻는다.
    public static Vector3 GetVerticalDir(Transform cam)
    {
        Vector3 vertical = cam.forward;
        vertical.y = 0;

        vertical = vertical.normalized;

        return vertical;
    }

    public static Vector3 GetHorizontalDir(Transform cam)
    {
        Vector3 horizontal = cam.right;
        horizontal.y = 0;

        horizontal = horizontal.normalized;

        return horizontal;
    }

    public static Transform[] GetChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }

    public static GameObject[] GetChildrenGameObject(Transform parent)
    {
        GameObject[] children = new GameObject[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i).gameObject;
        }

        return children;
    }

    public static float GetRigidFallTime(float fy, float deltaTime, float gravity, float fallHeight)
    {
        float fd = (fy * deltaTime);

        float tmp = Mathf.Sqrt(fd * fd + gravity * fallHeight);

        return (tmp - fd) / gravity;
    }

    public static Quaternion RandomRotationYaxis()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
    }

    public static int RandomSign()
    {
        return (int)Mathf.Sign(Random.Range(-1, 1));
    }

    public static int ToInt(object value)
    {
        return System.Convert.ToInt32(value);
    }

    public static float ToFloat(object value)
    {
        return System.Convert.ToSingle(value);
    }

    public void DelayedExecute(UnityAction action, float delay)
    {
        
    }

    IEnumerator _DelayedExecute(UnityAction action, float delay)
    {
        yield return new WaitForSeconds(delay);

        action();
    }
}
