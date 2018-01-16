vcl 4.0;
import directors;

backend service1 {
    .host = "mytwitter";
    .port = "80";
}

sub vcl_init {
    new bar = directors.round_robin();
    bar.add_backend(service1);
}

sub vcl_recv {
    set req.backend_hint = bar.backend();
}
