CREATE  FUNCTION set_updated()
    RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = now();
    RETURN NEW;
END;
$$ language 'plpgsql';



CREATE TABLE "user" (
                        id serial PRIMARY KEY,
                        username VARCHAR(50) UNIQUE NOT NULL,
                        email VARCHAR(100) UNIQUE NOT NULL,
                        password_hash VARCHAR(255) NOT NULL,
                        role VARCHAR(10) NOT NULL CHECK (role IN ('customer', 'chef', 'manager')),
                        created_at TIMESTAMP DEFAULT now(),
                        updated_at TIMESTAMP DEFAULT now()
);

CREATE TRIGGER "update_user"
    BEFORE UPDATE
    ON
        "user"
    FOR EACH ROW
EXECUTE PROCEDURE set_updated();


CREATE TABLE "session" (
                           id serial PRIMARY KEY,
                           user_id INT NOT NULL,
                           session_token VARCHAR(255) NOT NULL,
                           expires_at TIMESTAMP NOT NULL,
                           FOREIGN KEY (user_id) REFERENCES "user"(id)
);


CREATE TABLE "dish" (
                        id serial PRIMARY KEY,
                        name VARCHAR(100) NOT NULL,
                        description TEXT,
                        price DECIMAL(10, 2) NOT NULL,
                        quantity INT NOT NULL,
                        is_available BOOLEAN NOT NULL,
                        created_at TIMESTAMP DEFAULT now(),
                        updated_at TIMESTAMP DEFAULT now()
);

CREATE TRIGGER "update_dish"
    BEFORE UPDATE
    ON
        "dish"
    FOR EACH ROW
EXECUTE PROCEDURE set_updated();


CREATE TABLE "order" (
                       id serial PRIMARY KEY,
                       user_id INT NOT NULL,
                       status VARCHAR(50) NOT NULL,
                       special_requests TEXT,
                       created_at TIMESTAMP DEFAULT now(),
                       updated_at TIMESTAMP DEFAULT now(),
                       FOREIGN KEY (user_id) REFERENCES "user"(id)
);

CREATE TRIGGER "update_order"
    BEFORE UPDATE
    ON
        "order"
    FOR EACH ROW
EXECUTE PROCEDURE set_updated();

CREATE TABLE "order_dish" (
                              id serial PRIMARY KEY,
                              order_id INT NOT NULL,
                              dish_id INT NOT NULL,
                              quantity INT NOT NULL,
                              price DECIMAL(10, 2) NOT NULL,
                              FOREIGN KEY (order_id) REFERENCES "order"(id),
                              FOREIGN KEY (dish_id) REFERENCES "dish"(id)
);

CREATE TRIGGER "update_order_dish"
    BEFORE UPDATE
    ON
        "order_dish"
    FOR EACH ROW
EXECUTE PROCEDURE set_updated();