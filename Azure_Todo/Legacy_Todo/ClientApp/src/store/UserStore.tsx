import { Action, Reducer } from 'redux';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    user: any;
    token: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface SetuserAction { type: 'SET_USER', payload: any }
export interface LogoutAction { type: 'LOGOUT' }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = SetuserAction | LogoutAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    add: (obj: any) => ({ type: 'SET_USER', payload: obj } as SetuserAction),
    remove: () => ({ type: 'LOGOUT' } as LogoutAction)
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    let token = sessionStorage.getItem("btTasksToken");
    if (!token)
        token = '';
    if (state === undefined) {
        return { user: null, token: token };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SET_USER':   
            console.warn(action.payload);
            return { user: action.payload.user, token: action.payload.token };
        case 'LOGOUT':
            
            return { user: null, token: '' };
        default:
            return state;
    }
};
