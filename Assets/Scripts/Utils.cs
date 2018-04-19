using System;

public static class Utils
{
    public static Random randomGenerator = new Random();

    public static int BooleanToNumber(bool val)
    {
        if (val == true)
        {
            return 1;
        }
        return 0;
    }

    public static double GetRandomNumber(double minimum, double maximum)
    {
        return randomGenerator.NextDouble() * (maximum - minimum) + minimum;
    }

    public static double ReLU(double val, bool derivate)
    {
        if (val < 0)
        {
            return 0;
        }
        if (derivate)
        {
            return 1;
        }
        return val;
    }

    public static double Sigmoid(double val, bool derivate)
    {
        double sigResult = 1 / (1 + Math.Exp(-val));
        if (derivate)
        {
            sigResult *= (1 - sigResult);
        }
        return sigResult;
    }

    public static double LinearError(double delta)
    {
        return delta;
    }

    public static double HalfSquaredError(double delta)
    {
        return 0.5 * delta * delta;
    }

}
