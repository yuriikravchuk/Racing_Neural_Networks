# Racing Neural Networks

A small Unity racing game where the AI opponents learn to drive with a neural network written from scratch and trained by a genetic algorithm. No ML libraries are used.

Made in Unity 2021.3.

## How it works

**Training scene.** A population of cars drives the track at once. Each car is steered by its own neural network:

- **Inputs (10):** distances to the track borders from raycasts on the car, distance to the next checkpoint, current speed and the angle between the car and the checkpoint direction.
- **Network:** fully connected layers `10 → 4 → 2`, sigmoid in the hidden layer, tanh on the output.
- **Outputs (2):** throttle and steering.

A car scores points for each checkpoint it reaches in order and dies when it hits a border. Fitness is its score divided by the track's maximum. When the whole population is dead, the generation ends:

1. The best networks are kept as parents (elitism).
2. New networks are bred with uniform crossover: every weight and bias is taken from a random parent.
3. Each value can mutate with a small probability.

The best parents are spawned again in a different colour so you can watch them, and a time-scale slider speeds training up. Trained networks are saved to a binary file.

**Play scene.** You race against the trained cars after a countdown.

## Code layout

```
Assets/Scripts/AI                  Neuron, Layer, InputLayer, OutputLayer, NeuralNetwork
Assets/Scripts/AI/Training         AITrainer (generations), WeightsBalancer (selection, crossover, mutation)
Assets/Scripts/Car, Enemy, Player  shared car physics, AI driver, player driver
Assets/Scripts/Map                 track, checkpoints, borders, finish
Assets/Scripts/Game State Machine  menu, countdown and play states
Assets/Scripts/Save                binary save of trained networks
Assets/Scripts/Pool, UI, Starters  object pooling, UI, scene entry points
```

## Running

Open the project in Unity 2021.3, then start `Assets/Scenes/TrainingScene` to train or `Assets/Scenes/PlayScene` to race.
