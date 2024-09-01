from flask import Flask, request, jsonify
from requests import get, RequestException
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  

API_URL = "http://csharp:5000/api/meals/recipes?limit=40"  

def get_recipes():
    try:
        response = get(API_URL)
        response.raise_for_status()
        recipes = response.json()
        return recipes
    except RequestException as e:
        print(f"Error al obtener recetas: {e}")
        return []

@app.route('/search')
def search_recipes():
    keyword = request.args.get('keyword', '')

    recipes = get_recipes()

    if not recipes or not keyword:
        return jsonify({"message": "No recipes found or no keyword provided"}), 404

    descriptions = [recipe.get('description') for recipe in recipes]
    names = [recipe.get('name') for recipe in recipes]
    image_urls = [recipe.get('imageUrl') for recipe in recipes]

    vectorizer = TfidfVectorizer()
    tfidf_matrix = vectorizer.fit_transform(descriptions)
    
    query_vector = vectorizer.transform([keyword])

    similarities = cosine_similarity(query_vector, tfidf_matrix).flatten()

    sorted_recipes = sorted(
        zip(recipes, similarities),
        key=lambda x: x[1],
        reverse=True
    )

    result = [
        {
            "name": recipe.get('name'),
            "description": recipe.get('description'),
            "imageUrl": recipe.get('imageUrl'),
            "score": score
        } 
        for recipe, score in sorted_recipes if score > 0
    ]
    return jsonify(result)

if __name__ == '__main__':
    app.run(port=8000, host='0.0.0.0')
