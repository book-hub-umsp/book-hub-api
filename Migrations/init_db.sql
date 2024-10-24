CREATE DATABASE books_hub IF NOT EXISTS;

CREATE TYPE book_genre as ENUM (
    'epic_novel', 'novel', 'narrativa', 'story', 'essay', 'parable', 'lyrical_poem', 'elegy', 'epistle', 'epigram', 'ode', 'sonnet', 'comedy', 'tradegy', 'drama', 'poem', 'ballad');

CREATE TYPE book_status as ENUM ('draft', 'published', 'hiden', 'removed');

CREATE TABLE authors (
    id bigint GENERATED ALWAYS AS IDENTITY,
    name text NOT NULL,
    CONSTRAINT pk_authors PRIMARY KEY (id)
);

CREATE TABLE books (
    id bigint GENERATED ALWAYS AS IDENTITY,
    title text NOT NULL,
    author_id bigint NOT NULL,
    book_genre book_genre NOT NULL,
    book_annotation text NOT NULL,
    book_status book_status NOT NULL,
    creation_date timestamp with time zone NOT NULL,
    last_edit_date timestamp with time zone NOT NULL,
    CONSTRAINT pk_books PRIMARY KEY (id),
    CONSTRAINT fk_books_authors_id FOREIGN KEY (id) REFERENCES authors (id) ON DELETE CASCADE
);

CREATE TABLE key_words (
    id bigint GENERATED ALWAYS AS IDENTITY,
    content text NOT NULL,
    CONSTRAINT pk_key_words PRIMARY KEY (id)
);

CREATE TABLE keywords_links(
    key_word_id bigint NOT NULL,
    book_id bigint NOT NULL,
    CONSTRAINT fk_keywords_links_key_word_id FOREIGN KEY (key_word_id) REFERENCES key_words(id) ON DELETE CASCADE,
    CONSTRAINT fk_keywords_links_book_id FOREIGN KEY (book_id) REFERENCES books(id) ON DELETE CASCADE
);
