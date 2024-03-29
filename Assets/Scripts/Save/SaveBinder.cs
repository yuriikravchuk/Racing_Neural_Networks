﻿using AI;
using System.Collections.Generic;

public class SaveBinder
{
    private readonly ISaveProvider _provider;
    private const string _fileName = "save";

    public SaveBinder() => _provider = new BinarySaveProvider();

    public IReadOnlyList<Neuron[][]> Load()
    {
        List<Neuron[][]> save = _provider.TryGetSave<List<Neuron[][]>>(_fileName);
        return save;
    }

    public void Save(IReadOnlyList<TrainingResults> save)
    {
        List<Neuron[][]> results = new List<Neuron[][]>();
        foreach (TrainingResults result in save)
        {
            results.Add(result.Neurons);
        }

        _provider.UpdateSave(results, _fileName);
    }
}