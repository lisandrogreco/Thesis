import { Action, Reducer } from 'redux';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TasksState {
    tasks: any[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface AddTaskAction { type: 'ADD_TASK', payload: number }
export interface RemoveTaskAction { type: 'REMOVE_TASK' }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = AddTaskAction | RemoveTaskAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    add: (n: number) => ({ type: 'ADD_TASK', payload: n } as AddTaskAction),
    remove: () => ({ type: 'REMOVE_TASK' } as RemoveTaskAction)
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<TasksState> = (state: TasksState | undefined, incomingAction: Action): TasksState => {
    if (state === undefined) {
        return { tasks: [0,1,3] };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'ADD_TASK':
            let newTasks = state.tasks;
            newTasks.push(action.payload);
            return { tasks: newTasks  };
        case 'REMOVE_TASK':
            let oldTasks = state.tasks;
            oldTasks.pop( );
            return { tasks: oldTasks };
        default:
            return state;
    }
};
