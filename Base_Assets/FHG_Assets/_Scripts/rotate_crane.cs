using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_crane : MonoBehaviour
{
    public GameObject m_rotate_obj;
    //public string m_rotation_axis="z";
    public float m_rotation_speed = 0.08f;

    public float m_pause_min = 2.0f;
    public float m_pause_max = 5.0f;

    public float m_anim_min = 1.0f;
    public float m_anim_max = 5.0f;

    public float m_swifel_left = 0.0f;
    public float m_swifel_right = 90.0f;

    float m_rotation_x = 270.0f;
    float m_rotation_y = 0.0f;
    float m_rotation_z = 0.0f;

    float m_startTime;

    private int m_direction = 1;

    bool m_pause = false;
    float m_pause_time = 0.0f;

    //float m_animation_time;
    //float m_animation_timer = 5.0f;

    // Use this for initialization
    void Start()
    {

        //m_animation_time = m_animation_timer;

        if (m_rotate_obj != null)
        {
            Vector3 test = m_rotate_obj.transform.eulerAngles;
            int i = 0;
            m_rotation_z = m_swifel_left;
            //m_rotate_obj.transform.eulerAngles = new Vector3(m_rotate_obj.transform.eulerAngles.x, 0, m_rotate_obj.transform.eulerAngles.z);
            m_rotate_obj.transform.rotation = Quaternion.Euler(m_rotation_x, m_rotation_y, m_rotation_z);
        }
        m_startTime = Time.time;
        m_pause_time = Random.Range(m_pause_min, m_pause_max);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPause())
        {
            //rotateCrane();
            rotateCranSimple();
        }
    }

    bool isPause()


    {
        if (m_pause)
        {
            m_pause_time = m_pause_time - Time.deltaTime;

            if (m_pause_time < 0)
            {
                m_pause = false;
                m_startTime = Time.time;
                Debug.Log("Pause off: " + m_startTime);

                return false;
            }
            else
                return true;
        }
        else
            return false;
    }

    int RandomDirection()
    {
        float randomvalue = Random.value;

        if (randomvalue > 0.5f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    void rotateCranSimple()
    {
        if (m_rotate_obj != null)
        {

            
             m_rotation_z = (m_rotation_z + (1* m_direction))%360;

           // m_rotation_z = EaseInOutQuad(Time.time - m_startTime, m_swifel_left, m_swifel_right, (m_swifel_right - m_swifel_left) * m_rotation_speed);
           // Debug.Log("Time: " + (Time.time - m_startTime));


            if (m_rotation_z < m_swifel_left)
            {
                m_rotation_z = m_swifel_left;
                m_direction = m_direction * -1;
                enablePause();
            }

            if (m_rotation_z > m_swifel_right)
            {
                m_rotation_z = m_swifel_right;
                m_direction = m_direction * -1;
                enablePause();
            }

            m_rotation_z = m_rotation_z % 360;

            //   m_rotate_obj.transform.rotation=Quaternion.Euler(m_rotate_obj.transform.eulerAngles.x)
        //    Debug.Log("Rot: " + m_rotation_z);

            m_rotate_obj.transform.rotation = Quaternion.Euler(m_rotation_x, m_rotation_y, m_rotation_z);
        }
    }
    void enablePause()
    {
       
        m_pause_time = Random.Range(m_pause_min, m_pause_max);
        m_pause = true;
        Debug.Log("Enable Puase: " + m_pause_time);
    }

    //void rotateCrane()
    //{
    //    if (m_rotate_obj != null)
    //    {
    //        float easing_factor = EaseInOutQuint(m_animation_time, 0, 1, m_animation_timer);

    //        Debug.Log("easing_factor: " + easing_factor);

    //        float rot_Y = ((m_rotation_speed * Time.deltaTime * m_direction ) + m_rotate_obj.transform.eulerAngles.y) *easing_factor;

    //        if (rot_Y < m_swifel_left)
    //        {
    //            rot_Y = m_swifel_left;
    //            m_direction = m_direction * -1;
    //        }

    //        if (rot_Y > m_swifel_right)
    //        {
    //            rot_Y = m_swifel_right;
    //            m_direction = m_direction * -1;
    //        }

    //        m_rotate_obj.transform.eulerAngles = new Vector3(m_rotate_obj.transform.eulerAngles.x, rot_Y, m_rotate_obj.transform.eulerAngles.z);
    //    }
    //}

    /**
     * Easing equation float for a quadratic (t^2) easing in/out: acceleration until halfway, then deceleration.
     *
     * @param t		Current time (in frames or seconds).
     * @param b		Starting value.
     * @param c		Change needed in value.
     * @param d		Expected easing duration (in frames or seconds).
     * @return		The correct value.
     */
    public static float EaseInOutQuad(float current, float start, float change, float duration)
    {

        if ((current /= duration / 2) < 1) return change / 2 * current * current + start;

        return -change / 2 * ((--current) * (current - 2) - 1) + start;
    }

    /**
    * Easing equation float for a quadratic (t^2) easing out: decelerating to zero velocity.
    *
    * @param t		Current time (in frames or seconds).
    * @param b		Starting value.
    * @param c		Change needed in value.
    * @param d		Expected easing duration (in frames or seconds).
    * @return		The correct value.
*/
    public static float EaseOutQuad(float t, float b, float c, float d)
    {
        return -c * (t /= d) * (t - 2) + b;
    }

    /**
         * Easing equation float for a quintic (t^5) easing in/out: acceleration until halfway, then deceleration.
         *
         * @param t		Current time (in frames or seconds).
         * @param b		Starting value.
         * @param c		Change needed in value.
         * @param d		Expected easing duration (in frames or seconds).
         * @return		The correct value.
         */
    public static float EaseInOutQuint(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }
}
