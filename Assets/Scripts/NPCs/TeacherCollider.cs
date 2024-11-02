using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherCollider : MonoBehaviour
{
    public TeacherController teacherController;

    void Start()
    {
        teacherController = GetComponentInParent<TeacherController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tiro"))
        {
            teacherController.stunTeacher();
            Destroy(collision.gameObject);
            Debug.Log("Teacher hit by projectile");
        }
    }
}
