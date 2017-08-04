using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class L_System
{
    #region Alphabet
    public const char   c_counter_clockwise_    = '+';
    public const char   c_clockwise_            = '-';
    public const char   c_pitch_down_           = '&';
    public const char   c_pitch_up_             = '^';
    public const char   c_roll_left_            = '\\';
    public const char   c_roll_right_           = '/';
    public const char   c_turn_around_          = '|';
    public const char   c_push_                 = '[';
    public const char   c_pop_                  = ']';
    
    public const char   c_x_                    = 'X';
    public const char   c_y_                    = 'Y';
    #endregion

    public string                              axiom_          = "ab";
    public int                                 iterations_     = 5;

    public Dictionary<string, string>   rules_          = new Dictionary<string,string>();

    public L_System()
    {
        this.axiom_         = "F";
        add_rule            ("F=^F[-&\\G][\\++&G]||F[--&/G][+&G]");
        add_rule            ("G=&F[+G][-G]F[+G][-G]FG");
    }

    #region Rule Method
    public bool add_rule(string rule)
    {
        if (!rule.Contains('='))
            return false;

        string[]    parts   = rule.Split('=');
        if (parts.Length != 2)
            return false;
        if (!rules_.ContainsKey(parts[0]))
        {
            rules_.Add(parts[0], parts[1]);
            return true;
        }
        else
        {
            Debug.Log("Regel schon vorhanden");
        }


        return false;
    }

    public bool delete_rule_ (string rule_lhs)
    {
        if (rules_.ContainsKey(rule_lhs))
        {
            return rules_.Remove(rule_lhs);
        }
        return false;
    }

    public bool edit_rule(string lhs, string rhs)
    {
        if (lhs != string.Empty)
        {
            string rule;
            if (rules_.TryGetValue(lhs, out rule))
            {
                rules_[lhs] = rhs;
                return true;
            }
        }
        return false;
    }
    #endregion


    public string generate()
    {
        StringBuilder result        = new StringBuilder(axiom_);
        string axiom                = axiom_;

        for (int i = 0; i < iterations_; i++)
        {
            axiom   = result.ToString();

            result  = new StringBuilder();    

            for (int pos = 0; pos < axiom.Length; pos++)
            {
                char    c       = axiom[pos];
                string  rule;

                if (rules_.TryGetValue(c.ToString(), out rule))
                    result.Append(rule);
                else
                    result.Append(c);

                if (result.Length > 1000000)
                {
                    Debug.Log("String is to long");
                    break;
                }
            }
        }

        return result.ToString();
    }
}
