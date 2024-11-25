BEGIN;

CREATE TYPE book_status as ENUM('draft', 'published', 'hiden', 'removed');
 
CREATE TABLE genres (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    value TEXT NOT NULL,
	CONSTRAINT "pk_genres" 
        PRIMARY KEY (id)
);

CREATE TABLE books (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    title TEXT NOT NULL,
    author_id BIGINT DEFAULT NULL,
    genre_id BIGINT NOT NULL DEFAULT 1,
    annotation TEXT NOT NULL,
    "status" book_status NOT NULL DEFAULT 'draft',
    creation_date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    last_edit_date timestamp with time zone NOT NULL DEFAULT NOW(),
    CONSTRAINT "pk_books" 
        PRIMARY KEY (id),
    CONSTRAINT "fk_books_genre_id" 
        FOREIGN KEY (genre_id) REFERENCES genres(id) ON DELETE SET DEFAULT,
    CONSTRAINT "fk_books_author_id" 
        FOREIGN KEY (author_id) REFERENCES users(id) ON DELETE SET NULL
);

CREATE TABLE chapters (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    title TEXT NOT NULL,
    number BIGINT NOT NULL,
    book_id BIGINT NOT NULL,
    content TEXT NOT NULL,
    CONSTRAINT "pk_chapters" PRIMARY KEY(id),
    CONSTRAINT "fk_chapters_book_id" 
        FOREIGN KEY (book_id) REFERENCES books(id) ON DELETE CASCADE
);

INSERT INTO genres (value) 
VALUES 
    ('default'),
    ('epic_novel'),
    ('novel'),
    ('narrativa'),
    ('story'),
    ('essay'),
    ('parable'),
    ('lyrical_poem'),
    ('elegy'),
    ('epistle'),
    ('epigram'),
    ('ode'),
    ('sonnet'),
    ('comedy'),
    ('tradegy'),
    ('drama'),
    ('poem'),
    ('ballad');
    
CREATE TABLE favorites (
    "user_id" BIGINT NOT NULL,
    "book_id" BIGINT NOT NULL,
    CONSTRAINT "pk_favorites" 
        PRIMARY KEY ("user_id", "book_id"),
    CONSTRAINT "fk_favorites_user_id" 
        FOREIGN KEY ("user_id") REFERENCES "users" ("id") ON DELETE CASCADE,
    CONSTRAINT "fk_favorites_book_id" 
        FOREIGN KEY ("book_id") REFERENCES "books" ("id") ON DELETE CASCADE
);

CREATE TABLE keywords (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    value TEXT NOT NULL,
    CONSTRAINT "pk_keywords" PRIMARY KEY (id)
);

CREATE TABLE keywords_books_links (
    "keyword_id" BIGINT NOT NULL,
    "book_id" BIGINT NOT NULL,
    CONSTRAINT "pk_keywords_books_links"
        PRIMARY KEY ("keyword_id", "book_id"),
    CONSTRAINT "fk_keywords_books_links_keyword_id"
        FOREIGN KEY ("keyword_id") REFERENCES "keywords" ("id") ON DELETE CASCADE,
    CONSTRAINT "fk_keywords_books_links_book_id"
        FOREIGN KEY ("book_id") REFERENCES "books" ("id") ON DELETE CASCADE
);

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