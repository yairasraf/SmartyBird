using System;

public static class Utils
{
    public static Random randomGenerator = new Random();

    public static double ReLU(double val)
    {
        if (val < 0)
        {
            return 0;
        }
        return val;
    }

    public static double Sigmoid(double val)
    {

        return 1 / (1 + Math.Exp(-val));
    }
}
