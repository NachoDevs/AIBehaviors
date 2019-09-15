# AIBehaviors

## Description
The objective of this project is to create an AI system integrating State Machines and Behavior Trees in Unity.

## Integration
The problem with Behavior Trees is that they can get HUGE if we have a lot of different situations in which the AI can be in. So creating different Behavior Trees for different situations and have easy transitions between them would be a nice thing to have. This is where State Machines come into play, they are great to separate situations and make transitions between them, but not that good at presenting complex behaviors.

The idea here is to have a big State Machine where each state has its own Behavior Tree which will be in execution until we transition to another state (via reaching a leave node in the Behavior Tree(Task) that transitions to another state or by triggering a transition in the State Machine.

## Editors
There are two, very rough, editors created for an easier creation of this diagrams. You can find them in: Window>AI-Behaviors.
State Machine Editor:
![State Machine Editor Image](https://i.imgur.com/ulpnc22.png)

Behavior Tree Editor:
![State Machine Editor Image](https://i.imgur.com/A57CAwM.png)
![State Machine Editor Image](https://i.imgur.com/2FPdDle.png)
![State Machine Editor Image](https://i.imgur.com/qqxvEON.png)

Once we made our diagrams we export them into a JSON file, something like this:
```json
{
    "entryNode": {
        "nodeName": "DrinkingS",
        "nodeType": "Sequence",
        "inputs": [],
        "outputs": [
            ["DrinkingS", "Drinking"],
            ["DrinkingS", "DrinkingSS"]
        ]
    },
    "nodes": [{
        "nodeName": "Drinking",
        "nodeType": "Task",
        "inputs": [
            ["DrinkingS", "Drinking"]
        ],
        "outputs": []
    }, {
        "nodeName": "DrinkingSS",
        "nodeType": "Selector",
        "inputs": [
            ["DrinkingS", "DrinkingSS"]
        ],
        "outputs": [
            ["DrinkingSS", "GoIdle"],
            ["DrinkingSS", "GoEat"]
        ]
    }, {
        "nodeName": "GoIdle",
        "nodeType": "Task",
        "inputs": [
            ["DrinkingSS", "GoIdle"]
        ],
        "outputs": []
    }, {
        "nodeName": "GoEat",
        "nodeType": "Task",
        "inputs": [
            ["DrinkingSS", "GoEat"]
        ],
        "outputs": []
    }]
}
```
## Logic
Now we need to create the logic behind the Tasks and Selectors. We need to create functions in the brain class which have the same name as the Task or Selector name (we can re-use nodes assigning them the same name if they share functionality) Here some examples:
```cpp
    private int IdleS()
    {
        // We return the index of the selected node by a condition
        // For demonstration purposes
        return 1;
    }

    private int DrinkingSS()
    {
        // For demonstration purposes
        return 1;
    }

    private void Drinking()
    {
        print("I am Drinking!!");
    }

    private void Eating()
    {
        print("I am Eating!!");
    }

    private void Idling()
    {
        print("I am Idling!!");
    }

    private void GoDrink()
    {
        m_nextState = sm_states["Drink"];
        print("I am transitioning to Drink!!");
    }

    private void GoEat()
    {
        m_nextState = sm_states["Eat"];
        print("I am transitioning to Eat!!");
    }

    private void GoIdle()
    {
        m_nextState = sm_states["Idle"];
        print("I am transitioning to Idle!!");
    }
```
## Execution
Once we have all of the above we can press play and enjoy our new "intelligent" agent, or in this case our intelligent console.
![Example Image](https://i.imgur.com/bqHrYHO.png)

## Disclaimer
This is a very simple implementation and you may find bugs and errors. The project was done to understand in a more deep level how to implement State Machines and Behavior Trees.
