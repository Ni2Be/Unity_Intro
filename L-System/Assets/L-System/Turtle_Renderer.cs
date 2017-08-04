using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turtle_Renderer
{
    public float            angle_      = 5.0f;
    public float            distance_   = 1.0f;

    public Stack<Location>  stack_      = new Stack<Location>();

    public void process(string operations)
    {
        stack_                      = new Stack<Location>();
        Location    last_location   = new Location();
        Location    pos             = new Location();

        for (int i = 0; i < operations.Length; i++)
        {
            char    c               = operations[i];
            switch (c)
            {
                case L_System.c_clockwise_:
                    pos.turn_right(angle_);
                    break;
                case L_System.c_counter_clockwise_:
                    pos.turn_left(angle_);
                    break;
                case L_System.c_pitch_up_:
                    pos.pitch_up(angle_);
                    break;
                case L_System.c_pitch_down_:
                    pos.pitch_down(angle_);
                    break;
                case L_System.c_roll_left_:
                    pos.roll_left(angle_);
                    break;
                case L_System.c_roll_right_:
                    pos.roll_right(angle_);
                    break;
                case L_System.c_turn_around_:
                    pos.turn_around();
                    break;
                case L_System.c_push_:
                    stack_.Push(new Location(pos));
                    break;
                case L_System.c_pop_:
                    if (stack_.Count > 0)
                        pos = stack_.Pop();
                    break;
                case L_System.c_x_:
                    break;
                case L_System.c_y_:
                    break;
                default:
                    last_location.set_values(pos);
                    pos.move(distance_);
                    Gizmos.color = new Color(pos.position_.z / 200.0f, 0.0f, 0.0f);
                    Gizmos.DrawLine(
                                    last_location.position_,
                                    pos.position_);
                    break;
            }
        }

    }

}
