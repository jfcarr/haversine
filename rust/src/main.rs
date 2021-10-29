struct Coordinates {
    latitude_degrees: f64,
    longitude_degrees: f64,
}

impl Coordinates {
    fn latitude_radians(&self) -> f64 {
        return self.latitude_degrees.to_radians();
    }

    fn longitude_radians(&self) -> f64 {
        return self.longitude_degrees.to_radians();
    }
}

fn main() {
    let earth_radius_kilometer: f64 = 6371.0;

    let paris_coordinates = Coordinates {
        latitude_degrees: 48.85341,
        longitude_degrees: -2.34880,
    };
    let london_coordinates = Coordinates {
        latitude_degrees: 51.50853,
        longitude_degrees: -0.12574,
    };

    let delta_latitude =
        (paris_coordinates.latitude_degrees - london_coordinates.latitude_degrees).to_radians();
    let delta_longitude =
        (paris_coordinates.longitude_degrees - london_coordinates.longitude_degrees).to_radians();

    let central_angle_inner = (delta_latitude / 2.0).sin().powi(2)
        + paris_coordinates.latitude_radians().cos()
            * london_coordinates.latitude_radians().cos()
            * (delta_longitude / 2.0).sin().powi(2);
    let central_angle = 2.0 * central_angle_inner.sqrt().asin();

    let distance = earth_radius_kilometer * central_angle;

    println!(
        "Distance between Paris and London on the surface of Earth is {:.1} kilometers",
        distance
    );
}
