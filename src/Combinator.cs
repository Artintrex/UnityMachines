using System;

namespace MachineParts.Scripts
{
    public class Combinator : Machine
    {
        public enum Operator
        {
            GreaterThan,
            LessThan,
            Equal,
            GreaterThanOrEqual,
            LessThanOrEqual,
            IsNotEqual,
            OR,
            AND,
            XOR
        }

        public Signal signal1;
        public float value1;
        public Operator operatorType;
        public Signal signal2;
        public float value2;
        public Signal outputSignal;

        public bool booleanOutput;

        private void Update()
        {
            float v1 = signal1 == Signal.Value ? value1 : ReadIncomingSignal(signal1);
            float v2 = signal2 == Signal.Value ? value2 : ReadIncomingSignal(signal2);

            switch (operatorType)
            {
                case Operator.GreaterThan:
                    if (v1 > v2)
                    {
                        if (booleanOutput) UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.LessThan:
                    if (v1 < v2)
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.Equal:
                    if (Math.Abs(v1 - v2) < float.Epsilon)
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.GreaterThanOrEqual:
                    if (v1 >= v2)
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.LessThanOrEqual:
                    if (v1 <= v2)
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.IsNotEqual:
                    if (Math.Abs(v1 - v2) > float.Epsilon)
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.OR:
                    if (Convert.ToBoolean(v1) | Convert.ToBoolean(v2))
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.AND:
                    if (Convert.ToBoolean(v1) & Convert.ToBoolean(v2))
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
                case Operator.XOR:
                    if (Convert.ToBoolean(v1) ^ Convert.ToBoolean(v2))
                    {
                        if(booleanOutput)UpdateSignal(outputSignal, 1);
                        else UpdateSignal(outputSignal, ReadIncomingSignal(outputSignal));
                    }
                    else UpdateSignal(outputSignal, 0);
                    break;
            }
        }
    }
}
