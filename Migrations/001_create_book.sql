BEGIN;

CREATE DATABASE books_hub IF NOT EXISTS;

CREATE TYPE book_status as ENUM ('draft', 'published', 'hiden', 'removed');

CREATE TABLE genres (
    id bigint GENERATED ALWAYS AS IDENTITY,
    value text NOT NULL,
	CONSTRAINT pk_genres PRIMARY KEY (id));

CREATE TABLE books (
    id bigint GENERATED ALWAYS AS IDENTITY,
    title text NOT NULL,
    author_id bigint DEFAULT NULL,
    book_genre_id BIGINT NOT NULL DEFAULT 1,
    book_annotation text NOT NULL,
    book_status book_status NOT NULL,
    creation_date timestamp with time zone NOT NULL DEFAULT NOW(),
    last_edit_date timestamp with time zone NOT NULL,
    keywords_content json DEFAULT NULL,
    CONSTRAINT pk_books PRIMARY KEY (id),
    FOREIGN KEY (book_genre_id) REFERENCES genres (id) ON DELETE CASCADE
);

INSERT INTO genres (value) VALUES ('default');
INSERT INTO genres (value) VALUES ('epic_novel');
INSERT INTO genres (value) VALUES ('novel');
INSERT INTO genres (value) VALUES ('narrativa');
INSERT INTO genres (value) VALUES ('story');
INSERT INTO genres (value) VALUES ('essay');
INSERT INTO genres (value) VALUES ('parable');
INSERT INTO genres (value) VALUES ('lyrical_poem');
INSERT INTO genres (value) VALUES ('elegy');
INSERT INTO genres (value) VALUES ('epistle');
INSERT INTO genres (value) VALUES ('epigram');
INSERT INTO genres (value) VALUES ('ode');
INSERT INTO genres (value) VALUES ('sonnet');
INSERT INTO genres (value) VALUES ('comedy');
INSERT INTO genres (value) VALUES ('tradegy');
INSERT INTO genres (value) VALUES ('drama');
INSERT INTO genres (value) VALUES ('poem');
INSERT INTO genres (value) VALUES ('ballad');

CREATE FUNCTION update_change_books()
RETURNS trigger
LANGUAGE plpgsql
AS $function$
    BEGIN
        NEW.last_edit_date = NOW();
        RETURN NEW;
    END
$function$;
CREATE TRIGGER tr_update_change_books
    BEFORE UPDATE ON books
FOR EACH ROW EXECUTE PROCEDURE 
    update_change_books();

COMMIT;