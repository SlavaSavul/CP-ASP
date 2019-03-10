import { Film } from "./film.model";

export class Comment {
    id: string;
    description: string;
    filmId: string;
    userId: string;
    film: Film;
}