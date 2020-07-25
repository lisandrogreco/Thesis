"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    add: function (obj) { return ({ type: 'SET_USER', payload: obj }); },
    remove: function () { return ({ type: 'LOGOUT' }); }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
exports.reducer = function (state, incomingAction) {
    var token = sessionStorage.getItem("btTasksToken");
    if (!token)
        token = '';
    if (state === undefined) {
        return { user: null, token: token };
    }
    var action = incomingAction;
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
//# sourceMappingURL=UserStore.js.map