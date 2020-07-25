import * as React from 'react';
import { connect } from 'react-redux';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { ApplicationState } from '../store';
import { RouteComponentProps } from 'react-router';
import * as UserStore from '../store/UserStore';

type NavProps =
    UserStore.UserState &
    typeof UserStore.actionCreators;

export class NavMenu extends React.PureComponent<NavProps, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    logout = () => {
        this.props.remove(); 
        sessionStorage.removeItem("btTasksToken");
            }

    public render() {
        console.warn(this.props.token);
        let isHidden = !(this.props.token == '' || this.props.token == undefined);
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">AWS Todo</NavbarBrand>
                        <NavbarToggler onClick={this.toggle} className="mr-2"/>
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                                                             <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/tasks">Tasks</NavLink>
                                </NavItem>   
                                <NavItem hidden={isHidden}>
                                    <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                                </NavItem>
                                <NavItem hidden={isHidden}>
                                    <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>
                                </NavItem>
                                <NavItem hidden={!isHidden}>
                                    <NavLink tag={Link} className="text-dark" onClick={this.logout}>Logout </NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }

    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}


export default connect(
    (state: ApplicationState) => state.user,
    UserStore.actionCreators
)(NavMenu);
