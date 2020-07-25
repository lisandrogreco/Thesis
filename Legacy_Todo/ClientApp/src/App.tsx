import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import Login from './components/Login';
import Tasks from './components/Tasks';
import Register from './components/Register';

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path="/tasks" component={Tasks} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
    </Layout>
);
