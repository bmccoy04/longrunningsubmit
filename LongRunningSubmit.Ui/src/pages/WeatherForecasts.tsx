import {
  IonButtons,
  IonContent,
  IonHeader,
  IonMenuButton,
  IonPage,
  IonTitle,
  IonToolbar,
} from "@ionic/react";
import React, { useState, useEffect } from "react";
import { useParams } from "react-router";
import ExploreContainer from "../components/ExploreContainer";
import "./Page.css";

const WeatherForecasts: React.FC = () => {
  const [weatherForecasts, setWeatherForecasts] = useState<Array<any>>([]); //I'm a bad person
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<any>(null);
  useEffect(() => {
    fetch("https://localhost:5001/api/weather-forcast")
      .then((res) => res.json())
      .then((result) => {
        setWeatherForecasts(result);
        setLoading(false);
      })
      .catch((error) => {
        setError("it didn't work");
        setLoading(false);
      });
  }, []);

  return (
    <IonPage>
      <IonHeader>
        <IonToolbar>
          <IonButtons slot="start">
            <IonMenuButton />
          </IonButtons>
          <IonTitle>Weather Forecast Header</IonTitle>
        </IonToolbar>
      </IonHeader>

      <IonContent>
        <IonHeader collapse="condense">
          <IonToolbar>
            <IonTitle size="large">The Weather Forecast Page</IonTitle>
          </IonToolbar>
        </IonHeader>
        <div className="container">
          <strong>The Weather Forecast Page</strong>
          {loading && <div>Loading the data, please hold</div>}
          {!loading &&
            weatherForecasts.map((weather) => (
              <div key={weather.summary}>
                {weather.temperatureF} - {weather.summary}
              </div>
            ))}
          {!loading && error && <div>error: {error}</div>}
        </div>
      </IonContent>
    </IonPage>
  );
};

export default WeatherForecasts;
