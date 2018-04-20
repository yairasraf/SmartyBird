/// <summary>
/// This class represents a Bird DNA for a neural network
/// </summary>

using System.Collections.Generic;

[System.Serializable]
public class BirdDNA
{
    public List<List<List<double>>> weightsOfNeuralNetwork;
    public List<List<double>> biasesOfNeuralNetwork;


    public BirdDNA(List<List<List<double>>> weightsOfNeuralNetwork, List<List<double>> biasesOfNeuralNetwork)
    {
        this.weightsOfNeuralNetwork = weightsOfNeuralNetwork;
        this.biasesOfNeuralNetwork = biasesOfNeuralNetwork;
    }

    // call this function only once on each bird dna
    public void Mutate(double mutationChance)
    {
        for (int layerIndex = 0; layerIndex < weightsOfNeuralNetwork.Count; layerIndex++)
        {

            // mutating biases
            for (int biasIndex = 0; biasIndex < biasesOfNeuralNetwork[layerIndex].Count; biasIndex++)
            {
                // change to mutate here
                if (Utils.randomGenerator.NextDouble() < mutationChance)
                {
                    biasesOfNeuralNetwork[layerIndex][biasIndex] *= Utils.GetRandomNumber(-GameConstants.defaultMutationMultiplier, GameConstants.defaultMutationMultiplier);
                }
            }
            // mutating weights
            for (int nodeIndex = 0; nodeIndex < weightsOfNeuralNetwork[layerIndex].Count; nodeIndex++)
            {
                for (int weightIndex = 0; weightIndex < weightsOfNeuralNetwork[layerIndex][nodeIndex].Count; weightIndex++)
                {
                    // change to mutate here
                    if (Utils.randomGenerator.NextDouble() < mutationChance)
                    {
                        weightsOfNeuralNetwork[layerIndex][nodeIndex][weightIndex] *= Utils.GetRandomNumber(-GameConstants.defaultMutationMultiplier, GameConstants.defaultMutationMultiplier);
                    }
                }
            }
        }
    }
    // static function to cross over two bird dna
    // cross over meaning taking half dna of each bird
    public static BirdDNA CrossOver(BirdDNA bird1, BirdDNA bird2)
    {
        // validating data dimensions
        if (bird1.weightsOfNeuralNetwork.Count != bird2.weightsOfNeuralNetwork.Count)
        {
            throw new DataDimensionsMismatchException();
        }
        if (bird1.biasesOfNeuralNetwork.Count != bird2.biasesOfNeuralNetwork.Count)
        {
            throw new DataDimensionsMismatchException();
        }


        // initializing the new weights array
        // now doing cross over algorithm, taking 1 info from bird and 1 from bird2
        List<List<List<double>>> weightsInNewDNA = new List<List<List<double>>>();

        // looping each layer and adding its weights to the 2D array
        for (int curLayerIndex = 0; curLayerIndex < bird1.weightsOfNeuralNetwork.Count; curLayerIndex++)
        {
            List<List<double>> curLayerNodesDna = new List<List<double>>();
            // looping each weight in a specific layer and adding its weight to the 2D array
            for (int curNodeIndex = 0; curNodeIndex < bird1.weightsOfNeuralNetwork[curLayerIndex].Count; curNodeIndex++)
            {
                List<double> curNodeWeightsDna = new List<double>();
                for (int curNodeWeightIndex = 0; curNodeWeightIndex < bird1.weightsOfNeuralNetwork[curLayerIndex][curNodeIndex].Count; curNodeWeightIndex++)
                {
                    // add here a weight of either from bird1 or from bird2, one from each one or randomly
                    // returns a number which is between 0 and 1
                    if (Utils.randomGenerator.NextDouble() > 0.5)
                    {
                        curNodeWeightsDna.Add(bird1.weightsOfNeuralNetwork[curLayerIndex][curNodeIndex][curNodeWeightIndex]);
                    }
                    else
                    {
                        curNodeWeightsDna.Add(bird2.weightsOfNeuralNetwork[curLayerIndex][curNodeIndex][curNodeWeightIndex]);
                    }
                }
                curLayerNodesDna.Add(curNodeWeightsDna);
            }
            weightsInNewDNA.Add(curLayerNodesDna);
        }


        // initializing the new biases array
        List<List<double>> biasesInNewDNA = new List<List<double>>();
        // looping each layer in the neural network
        for (int curLayerIndex = 0; curLayerIndex < bird1.biasesOfNeuralNetwork.Count; curLayerIndex++)
        {
            List<double> biasesOfNodesInCurLayer = new List<double>();
            // looping each bias in a specific layer and adding its bias to the 1D array
            for (int curBiasIndex = 0; curBiasIndex < bird1.biasesOfNeuralNetwork[curLayerIndex].Count; curBiasIndex++)
            {
                // add here a weight of either from bird1 or from bird2, one from each one or randomly
                if (Utils.randomGenerator.Next(0, 2) == 1)
                {
                    biasesOfNodesInCurLayer.Add(bird1.biasesOfNeuralNetwork[curLayerIndex][curBiasIndex]);
                }
                else
                {
                    biasesOfNodesInCurLayer.Add(bird2.biasesOfNeuralNetwork[curLayerIndex][curBiasIndex]);
                }
            }
            biasesInNewDNA.Add(biasesOfNodesInCurLayer);
        }



        // creating and returning the new bird dna that was cross over between the two dna's
        BirdDNA birdDNAToReturn = new BirdDNA(weightsInNewDNA, biasesInNewDNA);
        return birdDNAToReturn;
    }

}