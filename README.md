# Neural Network Activity Simulation: Conway's Game of Life in Unity

## Overview

This Unity-based project is a simulation inspired by Conway's Game of Life, modeling the behavior of neurons in a neural network. The simulation features cells on a grid that activate and connect based on specific rules, divided into functional brain regions to mimic neurological activity variations. 

## Theoretical Notes


## Features

* **Core Mechanics:** Implements Conway's Game of Life with modified rules to simulate neuron activity. 
* **Zone-Based Modifications:** The grid is divided into four zones: two standard zones following Conway's rules and two special zones (Sensory Cortex and Hippocampus) with modified rules. 
* **User Interface:**
    * Start/Pause Button. 
    * Simulation Speed Slider. 
    * Generation Counter with an option for infinite generations. 
* **Grid Interaction:**
    * Draw Patterns: Users can toggle cells to create initial neuron arrangements. 
    * Zooming and Panning: Supports navigation of large grids.

## Game Mechanics & Rules

The simulation is based on the following rules:

* **Base Rules:**
    * Each cell (neuron) is either active (firing) or inactive (resting). 
    * Activation depends on the number of active neighbors. 
* **Zone-Based Modifications:**
    * **Standard Zones:** Follow original Game of Life rules.
    * **Sensory Cortex (Top-Right):** Increased activation probability; neurons activate with 2 or more active neighbors; living neurons survive with one, two, or three living neighbors; higher deactivation threshold. 
    * **Hippocampus (Bottom-Left):** Decreased activation probability; neurons need at least four active neighbors to activate; living neurons survive with exactly two living neighbors; lower deactivation threshold for stability. 
## Simulation Analysis

The simulation updates in discrete steps, with each frame representing a new generation. Neurons check their state and neighbors, apply activation rules, and update the grid.

* Standard zones show balanced activation/deactivation.
* The Sensory Cortex zone has faster, more chaotic activation with larger active clusters.
* The Hippocampus zone is more stable with smaller, isolated active groups. 

## Conclusions

This project successfully implemented and extended Conway's Game of Life in Unity. [cite: 56, 57] It demonstrates how modifying cellular automaton rules can simulate complex systems like neural networks, with different zones influencing overall behavior. 