# Stage 1: Building the code
FROM node:18 AS builder

WORKDIR /react-web

# Copy package.json and package-lock.json files
COPY package.json yarn.lock ./

# Install dependencies
RUN yarn install --frozen-lockfile

# Copy the rest of the code
COPY . .

# Build the project
RUN yarn build

# Stage 2: Run the built app
FROM node:18

WORKDIR /react-web

# Copy the built code from the builder stage
COPY --from=builder /react-web/dist ./dist
# Copy static assets and other necessary files
COPY public ./public

# Expose the port the app runs on
EXPOSE 3000

# Run the app
CMD ["npx", "serve", "-s", "dist"]
