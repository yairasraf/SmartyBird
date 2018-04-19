/// <summary>
/// This class represents a Bird DNA for a neural network
/// </summary>

using System.Collections.Generic;

[System.Serializable]
public class BirdDNA
{
    public List<List<double>> weightsOfNeuralNetwork;
    public List<double> biasesOfNeuralNetwork;

    public Bird bird;

    public BirdDNA(List<List<double>> weightsOfNeuralNetwork, List<double> biasesOfNeuralNetwork)
    {
        this.weightsOfNeuralNetwork = weightsOfNeuralNetwork;
        this.biasesOfNeuralNetwork = biasesOfNeuralNetwork;
    }

    // call this function only once on each bird dna
    public void Mutate()
    {
        // mutating biases
        for (int biasIndex = 0; biasIndex < biasesOfNeuralNetwork.Count; biasIndex++)
        {
            // change of 1/50 to mutate
            if (Utils.randomGenerator.Next(0, 50) == 25)
            {
                biasesOfNeuralNetwork[biasIndex] *= Utils.GetRandomNumber(0, 2);
            }
        }
        // mutating weights
        for (int layerIndex = 0; layerIndex < weightsOfNeuralNetwork.Count; layerIndex++)
        {
            for (int weightIndex = 0; weightIndex < weightsOfNeuralNetwork.Count; weightIndex++)
            {
                // change of 1/50 to mutate
                if (Utils.randomGenerator.Next(0, 50) == 25)
                {
                    weightsOfNeuralNetwork[layerIndex][weightIndex] *= Utils.GetRandomNumber(0, 2);
                }
            }
        }
    }
    // static function to cross over two bird dna
    // cross over meaning taking half dna of each bird
    public static BirdDNA CrossOver(BirdDNA bird1, BirdDNA bird2)
    {
        List<List<double>> weightsInNewDNA = new List<List<double>>();
        List<double> biasesInNewDNA = new List<double>();




        BirdDNA birdDNAToReturn = new BirdDNA(new List<List<double>>(), new List<double>());
        return birdDNAToReturn;
    }

}