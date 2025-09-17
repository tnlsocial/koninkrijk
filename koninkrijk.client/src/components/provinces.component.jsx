import { useEffect, useState } from "react";
import apiService from "../services/api.service";
import { MapContainer, GeoJSON, Marker, Tooltip, TileLayer } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import netherlandsProvinces from "../assets/provinces.json";
import { useLocation } from "wouter";
import * as turf from "turf";
import mapback from "../assets/background.jpg";
import L from 'leaflet';
import castle from "../assets/castle.svg"
import useAuthRedirect from "../hooks/authRedirect";

const Provinces = () => {
  useAuthRedirect();

  const [, setLocation] = useLocation();
  const [provinces, setProvinces] = useState(null);

  const handleProvinceClick = (provinceName) => {
    setLocation(`/provinces/${provinceName.properties.name}`);
  };

  const onEachFeature = (feature, layer) => {
    layer.on({
      click: () => handleProvinceClick(feature),
    });
  };

  const getCentroid = (feature) => {
    const centroid = turf.centroid(feature);
    return centroid.geometry.coordinates;
  };

  const getColor = (feature) => {
    //if(provinces && provinces.find((province) => province.name === feature.properties.name).currentPlayer !== null) return "#60d662";
    if (provinces && provinces.find((province) => province.name === feature.properties.name).provinceSize >= 8) return "#ea9999";
    if (provinces && provinces.find((province) => province.name === feature.properties.name).provinceSize >= 7) return "#ea9999";
    if (provinces && provinces.find((province) => province.name === feature.properties.name).provinceSize >= 5) return "#ffe599";
    if (provinces && provinces.find((province) => province.name === feature.properties.name).provinceSize >= 3) return "#b6d7a8";
    if (provinces && provinces.find((province) => province.name === feature.properties.name).provinceSize >= 0) return "#6aa84f";
    else return "#ffffff";
  };

  const markerIcon = L.icon({
    iconUrl: castle,
    iconSize: [30, 50]
  });

  const calcPoints = (date, provinceSize) => {
    const now = new Date();
    const lastCaptureDate = new Date(date);
    const diff = now - lastCaptureDate;
    const minutesPast = Math.floor(diff / (1000 * 60));
    const score = minutesPast * provinceSize;
    return score;
  }

  useEffect(() => {
    apiService
      .getProvinces()
      .then((response) => {
        setProvinces(response);
      })
      .catch((error) => {
        console.error("An error occurred while fetching the provinces:", error);
        setProvinces(null);
      });
  }, []);

  if (!provinces) {
    return null;
  }

  return (
    <div className="mx-auto col">
    <MapContainer
      style={{ height: "500px", width: "100%" }}
      zoom={7}
      center={[52.1326, 5.2913]}
    >
    <TileLayer url={mapback}/>

      <GeoJSON
        data={netherlandsProvinces}
        onEachFeature={onEachFeature}
        style={(feature) => ({
          color: "#000",
          weight: 0.5,
          fillColor: getColor(feature),
          fillOpacity: 0.85,
        })}
      />
      {netherlandsProvinces.features.map((feature, idx) => {
        const [lng, lat] = getCentroid(feature);
        if(provinces.find((province) => province.name === feature.properties.name).currentPlayer  !== null){
            const province = provinces.find((province) => province.name === feature.properties.name);
            const score = calcPoints(province.lastCapture, province.provinceSize)
            return (
            <Marker key={idx} position={[lat, lng]} icon={markerIcon}
                eventHandlers={{
                  mouseover: (event) => event.target.openPopup(),
                }}>
              <Tooltip direction="bottom" offset={[0, 20]} opacity={1}>
              <div>
                {provinces ? (
                <h4>
                  {province.currentPlayer
                  ? `${province.currentPlayer}`
                  : ""}
                </h4>
                ) : null}
                {provinces ? (
                <p>
                  <b>Veroverd sinds:</b>
                  <br />
                  {province.lastCapture
                  ? new Date(province.lastCapture).toLocaleString()
                  : ""}

                  <br />
                  <b>Vergaarde punten:</b>
                  <br />
                  {score
                  ? score
                  : "0"}
                </p>
                ) : null}
              </div>
              </Tooltip>
            </Marker>
            );
        }
      })}
    </MapContainer>
    </div>
  );
};

export default Provinces;
