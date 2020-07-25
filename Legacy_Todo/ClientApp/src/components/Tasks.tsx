import * as React from 'react';
import { store } from '..';
import { Button } from 'reactstrap';
import { API_URI } from '../Constants';

export default class Tasks extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = { items: [], newItemText: '', editItemText: '' , editIndex: '' };
    }

    componentDidMount() {
        this.loadData();
    }

    async loadData() {
        let token = store.getState().user.token;
        let response = await fetch(`${API_URI}/api/task`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ` + token
            }
        });

        if (response.status == 401) {
            this.props.history.push("/Login");
            return;
        }

        let json = await response.json();
        this.setState({ items: json });
    }

    async deleteTask(id: string) {
        let token = store.getState().user.token;
        let sendData = { Id: id };
        await fetch(`${API_URI}/api/task`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ` + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(sendData)
        });

        var newItems = this.state.items.filter((i: any) => i.id != id);
        this.setState({ items: newItems });

    }

    editTask = async (item: any) => {
        this.setState({ editIndex: item.id, editItemText: item.description });
    }

    saveTask = async (id: string) => {
        let token = store.getState().user.token;
        let sendData = { Description: this.state.editItemText, Id: id };
        await fetch(`${API_URI}/api/task`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ` + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(sendData)
        });
        let items = this.state.items;
        const itemIndex = items.findIndex((element: any) => element.id == id);
        items[itemIndex].description = this.state.editItemText ;
        this.setState({ items: items, newItemText: '', editIndex:'' });
    }

    addTask = async () => {
        let token = store.getState().user.token;
        let sendData = { Description: this.state.newItemText };
        await fetch(`${API_URI}/api/task`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ` + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(sendData)
        });
        let items = this.state.items;
        items.push({ id: 'refresh page', description: this.state.newItemText });
        this.setState({ items: items, newItemText: '' });
    }

    render() {
        return (
            <React.Fragment>
                <h1>My Tasks</h1>

                <p>This is a simple example of a React component.</p>
                <table>
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Description</th>
                            <th></th>
                            </tr>
                        </thead>
                            <tbody>
                                
                        {this.state.items && this.state.items.map((t: any) => this.state.editIndex == t.id ? <tr>
                            <td>{t.id}</td>
                           
                            <td>
                                <td><input type='text' value={this.state.editItemText} onChange={e => this.setState({ editItemText: e.target.value })} /></td>
                                </td>
                            
                            <td><Button onClick={e => this.saveTask(t.id)}>SAVE</Button></td>
                            <td><Button onClick={e => this.setState({ editItemText: '', editIndex: ''})}>Cancel</Button></td>
                            
                        </tr> : <tr>
                                <td>{t.id}</td>

                                <td>
                                    <td>{t.description}</td>
                                </td>
                                <td><Button onClick={e => this.editTask(t)}>EDIT</Button></td>
                                
                                <td><Button onClick={e => this.deleteTask(t.id)}>X</Button></td>
                                </tr>)}
                    </tbody>
                </table>
                <input type="text" value={this.state.newItemText} onChange={e => this.setState({ newItemText: e.target.value})} />
                <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={this.addTask}>
                    Add
                </button>
            </React.Fragment>
        );
    }
};
