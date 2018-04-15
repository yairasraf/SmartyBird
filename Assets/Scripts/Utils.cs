using System;

public static class Utils
{
    public static Random randomGenerator = new Random();

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
        if(derivate)
        {
            sigResult *= (1-sigResult);
        }
        return sigResult;
    }
}
