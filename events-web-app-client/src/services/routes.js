import LoginPage from '../pages/LoginPage';
import EventsListPage from '../pages/EventsListPage';
import EventDetailsPage from '../pages/EventDetailsPage';
import RegisterPage from '../pages/RegisterPage';
import AccountPage from '../pages/AccountPage';
import AccountEventsList from '../pages/AccountEventsListPage';
import CreateEventPage from '../pages/CreateEventPage';
import UpdateEventPage from '../pages/UpdateEventPage';

const routes = [
  { path: '/login', component: LoginPage },
  { path: '/events', component: EventsListPage },
  { path: '/events/:id', component: EventDetailsPage },
  { path: '/register', component: RegisterPage },
  { path: '/account/:id',component: AccountPage},
  { path: '/my-events',component: AccountEventsList},
  { path: '/new-event',component: CreateEventPage},
  { path: '/update-event/:eventId',component: UpdateEventPage},
];

export default routes;