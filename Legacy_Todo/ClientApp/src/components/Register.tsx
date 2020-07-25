import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as UserStore from '../store/UserStore';

class Register extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = { username: '', pw: '' };       

    }

    onRegister = async () => {
        const response = await fetch('https://localhost:44377/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                Username: this.state.username,
                Password: this.state.pw,
            })
        });
        if (response.status != 200) {
            
            alert(response.statusText);
        }
        let data = await response.json();
        UserStore.actionCreators.add({ user: data, token: data.token });

    }

    render() {
        return (<div>
            <h1>Register</h1>
            Username: <input type="text" value={this.state.username} onChange={e => this.setState({ username: e.target.value })} />
            Password: <input type="text" value={this.state.pw} onChange={e => this.setState({ pw: e.target.value })} />
            <input type="button" value="Login" onClick={this.onRegister} />

        </div>
        );
    }

}


export default connect()(Register);