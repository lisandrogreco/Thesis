import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import { RouteComponentProps } from 'react-router';
import * as UserStore from '../store/UserStore';
import { API_URI, KEY_PARAM } from '../Constants';

type LoginProps =
    UserStore.UserState &
    typeof UserStore.actionCreators &
    RouteComponentProps<{}>;

class Login extends React.Component<LoginProps, any> {

    constructor(props: any)
    {
        super(props);
        this.state = {username: '', pw:''};
    }

    onLogin = async () => {
        const response = await fetch(`${API_URI}/api/Login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'x-functions-key': KEY_PARAM
            },
           // credentials: 'include',
            body: JSON.stringify({
              Username: this.state.username,
              Password: this.state.pw,              
            })
        });

        let data = await response.json();
        this.props.add({ user: data, token: data.token }); 
        sessionStorage.setItem("btTasksToken", data.token);
        this.props.history.push("/");
    }

    render(){
        return(  <div>
            <h1>Login</h1>
            Username: <input type="text" value={this.state.username} onChange={e => this.setState({username: e.target.value})} />
            Password: <input type="text" value={this.state.pw} onChange={e => this.setState({pw: e.target.value})} />
            <input type="button" value="Login" onClick={this.onLogin} />

          </div>
        );
    }

}


export default connect((state: ApplicationState) => state.user,
    UserStore.actionCreators)(Login);